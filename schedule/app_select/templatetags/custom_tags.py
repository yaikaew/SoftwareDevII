from django import template


register = template.Library()


@register.filter
def get_list(dict, day):
    return dict[day]