from django import template
from ..views import day_start_times_used

register = template.Library()


@register.simple_tag
def get_dict_value(d) :
    return day_start_times_used[d]

@register.filter
def get_list(dict, day):
    return dict[day]